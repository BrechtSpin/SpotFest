import { HappeningArtist } from "./happening-artist";

export interface CreateHappening {
  name: string;
  startDate: string;
  endDate: string;
  happeningArtists: HappeningArtist[];
}

export interface UpdateHappeningWithArtists {
  guid: string;
  addHappeningArtists: HappeningArtist[];
}
