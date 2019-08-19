export class User {
    id: string;
    name: string;
    avatarLink: string;
    followingSlots : string[] = [];
    isAuthorized : boolean;
    roles : string[] = [];
    balance : number;
}