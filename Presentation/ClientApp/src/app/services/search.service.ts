import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Category } from 'src/app/models/category';
import { Tag } from 'src/app/models/tag';
import { slotMinimum } from 'src/app/models/slotMinimum';
import { Slot } from '../models/slot';
import { Page } from '../models/Page';

const PATH = 'https://localhost:44324/api/'

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  categories : Category[] = [];
  tags : Tag[] = [];
  selectedSlots  = new BehaviorSubject<slotMinimum[]>(null);
  selectedCategory = new BehaviorSubject<Category>(null);
  selectedTags = new BehaviorSubject<Tag[]>([]);
  page : number = 1;
  itemsOnPage : number = 10;
  numberOfPages? : number;
  isTags = false;
  isCategory = false;
  isName = false;
  name : string;
  currentTags : number[];
  currentCategory : number;

  constructor(private httpClient : HttpClient) { 
    let temp;
    this.selectedSlots.subscribe(val => temp = val);
    if(!temp){
      this.httpClient.get<Page>(PATH + 'Slot/page/1&' + this.itemsOnPage)
        .subscribe(val => {
          this.selectedSlots.next(val.slots);
          this.numberOfPages = parseInt(val.numberOfPages);
        });
    }

    this.selectedCategory.subscribe(val => this.isCategory = !!val);
    this.selectedTags.subscribe(val => this.isTags = val.length > 0 ? true : false);
  }

  createCategory(name : string) : Observable<any> {
    return this.httpClient.post<any>(PATH + 'Category', `"${name}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})})
  }
  
  createTag(name : string) : Observable<any> {
    let tag : Tag = {id : -1, name : name}
    return this.httpClient.post<any>(PATH + 'Tag', `"${name}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})})
  }

  getCategoryList() : Observable<Category[]> {
    return this.httpClient.get<Category[]>(PATH + 'Category');
  }

  getTagList() : Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(PATH + 'Tag');
  }

  getSlotsByCategory(id : number) {
    if(!this.isCategory){
      this.page = 1;
    }
    this.isName = false;
    this.isCategory = true;
    this.isTags = false;
    this.currentCategory = id;
    this.httpClient.post<Page>(PATH + 'Slot/byCategory/' + this.page + '&' + this.itemsOnPage,  this.currentCategory)
      .subscribe(val => {
        this.selectedSlots.next(val.slots);
        this.numberOfPages = parseInt(val.numberOfPages);
      },
      err => console.log(err.error));
  }

  getSlotsByTags(tags : number[]){
    if(!this.isTags){
      this.page = 1;
    }
    this.isName = false;
    this.isCategory = false;
    this.isTags = true;
    this.currentTags = tags;  
    this.httpClient.post<Page>(PATH + 'Slot/byTags/' + this.page + '&' + this.itemsOnPage, this.currentTags)
      .subscribe(val => {
        this.selectedSlots.next(val.slots);
        this.numberOfPages = parseInt(val.numberOfPages);
      },
      err => console.log(err.error));
  }

  getSlotsByName(name : string){
    if(!this.isName){
      this.page = 1;
    }
    this.isName = true;
    this.isCategory = false;
    this.isTags = false;
    this.name = name;
    this.httpClient.post<Page>(PATH + 'Slot/byName/' + this.page + '&' + this.itemsOnPage, `"${this.name}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})})
      .subscribe(val => {
        this.selectedSlots.next(val.slots);
        this.numberOfPages = parseInt(val.numberOfPages);
      },
      err => console.log(err.error));
  }

  getSlots(){
    this.httpClient.get<Page>(PATH + 'Slot/page/' + this.page + '&' + this.itemsOnPage)
        .subscribe(val => {
          this.selectedSlots.next(val.slots);
          this.numberOfPages = parseInt(val.numberOfPages);
        }, 
        err => console.log(err.error));
  }

  getSlot(id : string) : Observable<Slot> {
    return this.httpClient.get<Slot>(PATH + 'Slot/' + id);
  }

  next(){
    if(this.page + 1 <= this.numberOfPages){
      this.page++;
      if(this.isName){
        this.getSlotsByName(this.name);
      }
      else if(this.isTags){
        this.getSlotsByTags(this.currentTags);
      }
      else if(this.isCategory){
        this.getSlotsByCategory(this.currentCategory);
      }
      else{
        this.getSlots();
      }
    }
  }

  previous(){
    if(this.page - 1 > 0){
      this.page--;
      if(this.isName){
        this.getSlotsByName(this.name);
      }
      else if(this.isTags){
        this.getSlotsByTags(this.currentTags);
      }
      else if(this.isCategory){
        this.getSlotsByCategory(this.currentCategory);
      }
      else{
        this.getSlots();
      }
    }
  }

  refreshTags(){
    let temp;
    this.selectedTags.subscribe(val => temp = val);
    if(temp.length < 1){
      this.getTagList().subscribe(val => this.tags = val);
    }
  }

  refreshCategories(){
    let temp;
    this.selectedCategory.subscribe(val => temp = val);
    if(!temp){
      this.getCategoryList().subscribe(val => this.categories = val);
    }
  }

  refresh(){
    this.refreshTags();
    this.refreshCategories();
  }

  updatePrice(slotId : string) : Observable<number>{
    return this.httpClient.get<number>(PATH + 'Slot/' + slotId + '/price');
  }

  getUserBet(slotId : string) : Observable<string>{
    return this.httpClient.get<string>(PATH + 'Slot/' + slotId + '/userBet');
  }
}
