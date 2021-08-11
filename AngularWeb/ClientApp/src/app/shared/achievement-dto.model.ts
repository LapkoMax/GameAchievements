export class AchievementDto {
  id: number;
  name: string;
  description: string;
  condition: string;
  constructor() {
    this.id = 0;
    this.name = "";
    this.description = "";
    this.condition = "";
  }
}
