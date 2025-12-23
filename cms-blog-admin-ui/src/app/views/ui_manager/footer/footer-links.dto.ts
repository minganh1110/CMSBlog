export interface FooterLinkDto {
  id?: string;
  label: string;
  url: string;
  icon?: string;
  targetBlank: boolean;
  sortOrder: number;
  isActive: boolean;
}
