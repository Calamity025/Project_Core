import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchService } from 'src/app/services/search.service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/services';
import { Slot } from 'src/app/models/slot';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Category } from 'src/app/models/category';
import { Tag } from 'src/app/models/tag';
import { User } from 'src/app/models';

export interface SlotUpdate{
  Name : string,
  Description : string,
  CategoryId  : number,
  SlotTagIds : number[]
}

@Component({
  selector: 'app-slot-edit',
  templateUrl: './slot-edit.component.html',
  styleUrls: ['./slot-edit.component.css']
})
export class SlotEditComponent implements OnInit {
  slotId? : string;
  slot? : Slot;
  previewLink? : string;
  fileName = "Choose file";
  uploadForm: FormGroup; 
  selectedCategory? : Category;
  selectedTags? : Tag[] = [];
  selectedStatus? : string;
  capacity = 5;
  currentCount = 0;
  tempTag? : string;
  isAddTagPressed : boolean = false;
  isTagsValid : boolean = true;
  description? : string;
  title? : string;
  currentUser? : User;


  constructor(private route: ActivatedRoute,
    private router : Router,
    private searchService : SearchService,
    private httpClient : HttpClient,
    private authService : AuthService,
    private formBuilder : FormBuilder) { }

  ngOnInit() {
    this.slotId = this.route.snapshot.paramMap.get('id');
    this.searchService.getSlot(this.slotId).subscribe(val => {
      this.slot = val;
      this.selectedTags = this.slot.slotTags;
      this.currentCount = this.slot.slotTags.length;
      this.selectedCategory = this.slot.category;
    })
    this.uploadForm = this.formBuilder.group({
      profile: ['']
    });
    this.authService.currentUser$.subscribe(val => this.currentUser = val);
    if(this.searchService.tags.length == 0 || this.searchService.categories.length == 0) {
      this.searchService.refresh();
    }
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

  onTagInput(name : string){
    this.tempTag = name;
  }

  onAddTagClick(){
    if(!!!this.selectedTags.find(t => t.name == this.tempTag) && !!this.tempTag){
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

  onBackClick(){
    this.router.navigate(['/slot/' + this.slotId]);
  }

  onSubmitClick(){
    let slot : SlotUpdate ={
      Name : this.title,
      CategoryId : this.selectedCategory.id,
      SlotTagIds : this.selectedTags.map(x => x.id),
      Description : this.description
    }

    this.httpClient.put<any>('https://localhost:44324/api/Slot/' + this.slotId, slot)
      .subscribe(val => {
        if(this.uploadForm.get('profile').value){
          const formData = new FormData();
          formData.append('file', this.uploadForm.get('profile').value);
          this.httpClient.put<any>('https://localhost:44324/api/Slot/image/' + this.slotId, formData)
          .subscribe(val => {
            if(val){
              this.router.navigate(['/slot/'+this.slotId]);
              }
            })
        }
        else{
          this.router.navigate(['/slot/'+this.slotId]);
        }
    });
  }

  onDescriptionInput(value : string){
    this.description = value;
  }

  onTitleInput(title : string){
    this.title = title;
  }
}
