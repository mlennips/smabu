
export interface ErrorDetail {
  code?: string | null;
  description?: string | null;
  details?: { [key: string]: string } | null;
}
