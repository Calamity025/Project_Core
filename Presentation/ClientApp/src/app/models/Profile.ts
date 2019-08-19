import { slotMinimum } from "./slotMinimum";

export interface Profile {
    firstName : string;
    lastName : string;
    imageLink : string;
    followingSlots : slotMinimum[];
    betSlots : slotMinimum[];
    placedSlots : slotMinimum[] ;
    wonSlots : slotMinimum[];
}