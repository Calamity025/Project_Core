import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/models/category';
import { Tag } from 'src/app/models/tag';
import { SearchService } from '../search.service';

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

  constructor(private searchService : SearchService) { }

  ngOnInit() {
    this.searchService.refresh();
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
    this.isFormActivated = true;
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
      this.selectedCategory = null;
    else
      this.selectedCategory = category;
  }

  onTagSelect(tag : Tag){
    if(this.selectedTags.indexOf(tag) != -1){
      let i = this.selectedTags.indexOf(tag);
      this.selectedTags.splice(i, 1);
    }
    else{
      this.selectedTags.push(tag);
    }
  }
}
