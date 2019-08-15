import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from 'src/app/models/category';
import { Tag } from 'src/app/models/tag';

const PATH = 'https://localhost:44324/api/'

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  categories : Category[] = [];
  tags : Tag[] = [];

  constructor(private httpClient : HttpClient) { }

  createCategory(name : string) : Observable<Category> {
    let category : Category = {id : -1, name : name}
    return this.httpClient.post<Category>(PATH + 'Category', category);
  }
  
  createTag(name : string) : Observable<Tag> {
    let tag : Tag = {id : -1, name : name}
    return this.httpClient.post<Category>(PATH + 'Tag', tag);
  }

  getCategoryList() : Observable<Category[]> {
    return this.httpClient.get<Category[]>(PATH + 'Category');
  }

  getTagList() : Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(PATH + 'Tag');
  }

  refreshTags(){
    this.getTagList().subscribe(val => this.tags = val);
  }

  refreshCategories(){
    this.getCategoryList().subscribe(val => this.categories = val);
  }

  refresh(){
    this.refreshTags();
    this.refreshCategories();
  }
}
