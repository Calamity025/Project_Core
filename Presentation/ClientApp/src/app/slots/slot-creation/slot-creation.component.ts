import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { SearchService } from 'src/app/services/search.service';
import { Tag } from 'src/app/models/tag';
import { SlotCreationModel } from 'src/app/models/SlotCreationModel';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services';
import { Category } from 'src/app/models/category';

@Component({
  selector: 'app-slot-creation',
  templateUrl: './slot-creation.component.html',
  styleUrls: ['./slot-creation.component.css']
})
export class SlotCreationComponent implements OnInit {
  title? : string;
  price? : number;
  previewLink = '';
  fileName = "Choose file";
  uploadForm: FormGroup; 
  selectedCategory?;
  selectedTags? : Tag[] = [];
  capacity = 5;
  currentCount = 0;
  tempTag? : string;
  isAddTagPressed : boolean = false;
  step? : number;
  description? : string;
  isTagsValid : boolean = true;
  isAuthorized? = false;
  
  constructor(private formBuilder: FormBuilder,
    private searchService : SearchService,
    private httpClient : HttpClient,
    private router : Router,
    private authService : AuthService) {
      authService.isAuthorized$.subscribe(
        val => this.isAuthorized = val);
     }

  ngOnInit() {
    this.uploadForm = this.formBuilder.group({
      profile: ['']
    });
    if(this.searchService.tags.length == 0 || this.searchService.categories.length == 0) {
      this.searchService.refresh();
    }
  }

  onTitleInput(title : string){
    this.title = title;
  }

  onPriceInput(price : number) {
    this.price = price;
  }

  onSubmitClick(){
    let slot : SlotCreationModel ={
      name : this.title,
      price : this.price,
      categoryId : this.searchService.categories.find(x=>x.name==this.selectedCategory).id,
      slotTagIds : this.selectedTags.map(x => x.id),
      step : this.step,
      description : this.description
    }

    this.httpClient.post<number>('https://localhost:44324/api/Slot', slot)
      .subscribe(val => {
        if(val && this.uploadForm.get('profile').value){
          const formData = new FormData();
          formData.append('file', this.uploadForm.get('profile').value);
          this.httpClient.post<any>('https://localhost:44324/api/Slot/image/' + val, formData)
          .subscribe(val => {
            if(val){
              this.router.navigate(['/']);
              this.searchService.getSlots();
              }
            },
            err => alert(err.error))
        }
        else if(val){
          this.router.navigate(['/']);
          this.searchService.getSlots();
        }
    },
    err => alert(err.error));
  }

  onTagInput(name : string){
    this.tempTag = name;
  }

  onDescriptionInput(value : string){
    this.description = value;
  }

  onAddTagClick(){
    if(!!!this.selectedTags.find(t => t.name == this.tempTag) && this.currentCount < this.capacity && !!this.tempTag){
      this.selectedTags.push(this.searchService.tags.find(t => t.name == this.tempTag));
      this.isAddTagPressed = false;
      this.currentCount++;
      this.isTagsValid = true;
    }
    else{
      this.isTagsValid = false;
    }
  }

  onAddTagActivateClick(){
    this.isAddTagPressed = !this.isAddTagPressed;
  }

  onRemoveTagClick(tag : Tag){
    this.selectedTags.splice(this.selectedTags.indexOf(tag), 1);
    this.currentCount--;
  }

  onCloseAddTagClick() {
    this.isAddTagPressed = false;
  }

  onStepInput(value : number){
    this.step = value;
  }

  onFileSelect(event) {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      this.uploadForm.get('profile').setValue(file);
      var reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]); 
      reader.onload = (event:any) => { 
        this.previewLink = event.target.result;
      }
      this.fileName = file.name;
    }
  }
}
