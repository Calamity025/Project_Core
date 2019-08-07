import { User } from "./user";

export interface UserResponse {
    access_token : string;
    user : User
}