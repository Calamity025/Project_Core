import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/models/category';
import { Tag } from 'src/app/models/tag';
import { SearchService } from '../../../services/search.service';
import { AuthService } from 'src/app/services';
import { User } from 'src/app/models';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-search-property',
  templateUrl: './search-property.component.html',
  styleUrls: ['./search-property.component.css']
})
export class SearchComponent implements OnInit {
  isSwitcherOn : boolean = true;
  isFormActivated: boolean = false;
  newTagName? : string;
  newCategoryName? : string;
  selectedCategory? : Category;
  selectedTags? : Tag[] = [];
  isAuthorized? : boolean;
  currentUser? : User;
  isEditCategoryOn? = false;
  editNameCategory? : string;
  editCategory? : Category;
  editTag? : Tag;
  editNameTag? : string;
  isEditTagOn? = false;

  constructor(private searchService : SearchService,
    private authService : AuthService,
    private httpClient : HttpClient) { }

  ngOnInit() {
    this.authService.isAuthorized$.subscribe(val => this.isAuthorized = val);
    this.authService.currentUser$.subscribe(val => this.currentUser = val);
    this.searchService.selectedCategory.subscribe(val => this.selectedCategory = val);
    this.searchService.selectedTags.subscribe(val => this.selectedTags = val);
  }

  onCategoryClick(){
    this.isSwitcherOn = true;
    this.isFormActivated = false;
  }

  onTagClick(){
    this.isSwitcherOn = false;
    this.isFormActivated = false;
  }

  onAddButtonClick(){
    this.isFormActivated = !this.isFormActivated;
  }

  onTagAdd(){
    this.searchService.createTag(this.newTagName)
      .subscribe(() => {this.searchService.refreshTags(); this.isFormActivated = false;},
      err => alert(err.error));
  }

  onCategoryAdd(){
    this.searchService.createCategory(this.newCategoryName)
      .subscribe(() => {this.searchService.refreshCategories(); this.isFormActivated = false; },
      err => alert(err.error));
  }

  onCategoryNameInput(value : string) {
    this.newCategoryName = value;
  }

  onTagNameInput(value : string) {
    this.newTagName = value;
  }

  onCategorySelect(category : Category){
    if(this.selectedCategory == category)
    {
      this.searchService.selectedCategory.next(null);
      this.searchService.getSlots();
    }
    else
    {
      this.searchService.selectedTags.next([]);
      this.searchService.selectedCategory.next(category);
      this.searchService.getSlotsByCategory(this.selectedCategory.id);
    }
  }

  onTagSelect(tag : Tag){
    if(this.selectedTags.indexOf(tag) != -1){
      let i = this.selectedTags.indexOf(tag);
      this.selectedTags.splice(i, 1);
      this.searchService.selectedTags.next(this.selectedTags);
      if(this.selectedTags.length == 0){
        this.searchService.getSlots();
      }
      else{
        this.searchService.getSlotsByTags(this.selectedTags.map(x => x.id));
      }
    }
    else{
      this.selectedTags.push(tag);
      this.searchService.selectedCategory.next(null);
      this.searchService.selectedTags.next(this.selectedTags);
      this.searchService.getSlotsByTags(this.selectedTags.map(x => x.id));
    }
  }

  onEditTagOn(tag : Tag){
    this.isEditTagOn = true;
    this.editTag = tag;
  }

  onTagEditNameInput(value : string){
    this.editNameTag = value;
  }

  onTagEdit(){
    this.httpClient.put<any>('https://localhost:44324/api/Tag/' + this.editTag.id, `"${this.editNameTag}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})})
    .subscribe(() => {
      this.searchService.refreshTags();
      this.isEditTagOn = false;
    },
    err => alert(err.error)); 
  }

  onEditCategoryOn(category : Category){
    this.isEditCategoryOn = true;
    this.editCategory = category;
  }

  onCategoryEditNameInput(value : string){
    this.editNameCategory = value;
  }

  onCategoryEdit(){
    this.httpClient.put<any>('https://localhost:44324/api/Category/' + this.editCategory.id, `"${this.editNameCategory}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})})
    .subscribe(() => {
      this.searchService.refreshCategories();
      this.isEditCategoryOn = false;
    },
    err => alert(err.error)); 
  }

  onDeleteTag(tag : Tag){
    this.httpClient.delete<any>('https://localhost:44324/api/Tag/' + tag.id)
    .subscribe(() => this.searchService.refreshTags(),
    err => alert(err.error));
  }
}
