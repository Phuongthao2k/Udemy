import { Photo } from "./photo";

export interface Member {
    id: number;
    userName: string;
    dateOfBirth: string;
    knowAs?: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    photoUrl: string;
    country: string;
    photos: Photo[];
    age: number;
}

