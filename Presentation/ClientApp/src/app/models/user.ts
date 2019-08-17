export class User {
    Id: string;
    Name: string;
    AvatarLink: string;
    FollowingSlots : string[] = [];
    IsAuthorized : boolean;
    Roles : string[] = [];
}