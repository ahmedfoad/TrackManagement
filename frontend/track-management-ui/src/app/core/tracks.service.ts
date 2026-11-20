import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Track, TrackDetails, TrackStatus } from './track.models';

@Injectable({ providedIn: 'root' })
export class TracksService {
  private readonly http = inject(HttpClient);

  list(status: TrackStatus | '') {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }
    return this.http.get<Track[]>('/api/tracks', { params });
  }

  get(id: number) {
    return this.http.get<TrackDetails>(`/api/tracks/${id}`);
  }
}
