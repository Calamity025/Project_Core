import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/models/category';
import { Tag } from 'src/app/models/tag';
import { SearchService } from '../../../services/search.service';
import { AuthService } from 'src/app/services';
import { User } from 'src/app/models';

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

  constructor(private searchService : SearchService,
    private authService : AuthService) { }

  ngOnInit() {
    this.authService.isAuthorized$.subscribe(val => this.isAuthorized = val);
    this.authService.currentUser$.subscribe(val => this.currentUser = val);
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
    .subscribe(() => {this.searchService.refreshTags(); this.isFormActivated = false;});
  }

  onCategoryAdd(){
    this.searchService.createCategory(this.newCategoryName)
    .subscribe(() => {this.searchService.refreshCategories(); this.isFormActivated = false; });
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
      this.selectedCategory = null;
      this.searchService.getSlots();
    }
    else
    {
      this.selectedCategory = category;
      this.searchService.getSlotsByCategory(this.selectedCategory.id);
    }
  }

  onTagSelect(tag : Tag){
    if(this.selectedTags.indexOf(tag) != -1){
      let i = this.selectedTags.indexOf(tag);
      this.selectedTags.splice(i, 1);
      if(this.selectedTags.length == 0){
        this.searchService.getSlots();
      }
      else{
        this.searchService.getSlotsByTags(this.selectedTags.map(x => x.id));
      }
    }
    else{
      this.selectedTags.push(tag);
      this.searchService.getSlotsByTags(this.selectedTags.map(x => x.id));
    }
  }
}
