import { ArtistSummary } from "./artist-summary";

export interface HappeningFull {
  guid: string;
  name: string;
  slug: string;
  startDate: string;
  endDate: string;
  artistSummaries: ArtistSummary[]
}
