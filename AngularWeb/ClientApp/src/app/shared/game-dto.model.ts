export class GameDto {
  id: number;
  name: string;
  description: string;
  rating: number;
  genres: string;
  constructor() {
    this.id = 0;
    this.name = "";
    this.description = "";
    this.rating = 0.0;
    this.genres = "";
  }
}
