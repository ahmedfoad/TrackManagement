export type TrackStatus = 'Draft' | 'Submitted' | 'Distributed';
export type DistributionStatus = 'Pending' | 'Live' | 'Rejected';

export interface Track {
  id: number;
  title: string;
  artistId: number;
  artistName: string;
  iSrc: string;
  releaseDate: string;
  genre: string;
  status: TrackStatus;
}

export interface TrackDistribution {
  dspId: number;
  dspName: string;
  status: DistributionStatus;
  submittedAt: string;
}

export interface TrackDetails extends Track {
  distributions: TrackDistribution[];
}

export interface LoginResponse {
  token: string;
  email: string;
  username: string;
  roles: string[];
}
