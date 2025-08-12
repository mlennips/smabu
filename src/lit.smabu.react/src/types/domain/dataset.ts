
import { Serie } from './serie';

export interface Dataset {
  series?: Array<Serie> | null;
  valueLabels?: Array<string> | null;
}
