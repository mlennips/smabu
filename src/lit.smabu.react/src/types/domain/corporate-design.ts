
import { Color } from './color';
import { FileReference } from './file-reference';

export interface CorporateDesign {
  brand?: string | null;
  shortName?: string | null;
  slogan?: string | null;
  color1?: Color;
  color2?: Color;
  logo?: FileReference;
}
