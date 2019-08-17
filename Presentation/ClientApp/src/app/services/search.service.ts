import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from 'src/app/models/category';
import { Tag } from 'src/app/models/tag';
import { slotMinimum } from 'src/app/models/slotMinimum';
import { Slot } from '../models/slot';

const PATH = 'https://localhost:44324/api/'

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  categories : Category[] = [];
  tags : Tag[] = [];
  selectedSlots : slotMinimum[] = [];
  page : number = 1;
  itemsOnPage : number = 5;

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

  getSlotsByCategory(id : number){
    this.httpClient.post<slotMinimum[]>(PATH + 'Slot/byCategory/' + this.page + '&' + this.itemsOnPage, id)
      .subscribe(val => this.selectedSlots = val);
  }

  getSlotsByTags(tags : number[]){
    this.httpClient.post<slotMinimum[]>(PATH + 'Slot/byTags/' + this.page + '&' + this.itemsOnPage, tags)
      .subscribe(val => this.selectedSlots = val);
  }

  getSlotsByName(name : string){
    this.httpClient.post<slotMinimum[]>(PATH + 'Slot/byName/' + this.page + '&' + this.itemsOnPage, `"${name}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})}).subscribe(val => this.selectedSlots = val);
  }

  getSlots(){
    this.httpClient.get<slotMinimum[]>(PATH + 'Slot/page/' + this.page + '&' + this.itemsOnPage)
      .subscribe(val => this.selectedSlots = val);
  }

  getSlot(id : string) : Observable<Slot> {
    return this.httpClient.get<Slot>(PATH + 'Slot/' + id);
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
    this.getSlots();
  }

  updatePrice(slotId : string) : Observable<number>{
    return this.httpClient.get<number>(PATH + 'Slot/' + slotId + '/price');
  }

  getUserBet(slotId : string) : Observable<number>{
    return this.httpClient.get<number>(PATH + 'Slot/' + slotId + '/userBet');
  }
}
