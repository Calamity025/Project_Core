import { Category } from "./category";
import { Tag } from "./tag";
import { User } from "./user";
import { UserSlot } from "./userSlot";

export interface Slot {
    name : string;
    price : number;
    step : number;
    endTime : string;
    status : string;
    category : Category;
    slotTags : Tag[];
    description : string;
    imageLink : string;
    user : UserSlot;
}