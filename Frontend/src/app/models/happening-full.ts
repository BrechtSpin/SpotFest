import { ArtistSummary } from "./artist-summary";

export interface HappeningFull {
  name: string;
  slug: string;
  startDate: string;
  endDate: string;
  artistSummaries: ArtistSummary[]
}
