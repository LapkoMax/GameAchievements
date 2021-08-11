export class MetaData {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
  constructor() {
    this.currentPage = 1;
    this.totalPages = 1;
    this.pageSize = 0;
    this.totalCount = 0;
    this.hasPrevious = false;
    this.hasNext = false;
  }
}
