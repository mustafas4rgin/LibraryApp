import { Role } from "./Role";

export interface User {
    id: number;
    name: string;
    email: string;
    username: string;
    address: string;
    role : Role | null
  }
  