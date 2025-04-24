import { HappeningArtist } from "./happening-artist";

export interface Happening {
  name: string;
  startDate: string;
  endDate: string;
  happeningArtists: HappeningArtist[];
}
