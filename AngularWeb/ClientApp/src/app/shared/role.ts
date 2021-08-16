import { Guid } from "guid-typescript";

export class Role {
  id: string;
  name: string;
  constructor() {
    this.id = Guid.create().toString();
    this.name = "";
  }
}
