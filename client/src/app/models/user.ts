import { UserAddress } from "./user-adress";

export interface User {
    userName: string;
    email: string;
    phoneNumber: string;
    addresses: UserAddress[];
    accessToken: string;
}
