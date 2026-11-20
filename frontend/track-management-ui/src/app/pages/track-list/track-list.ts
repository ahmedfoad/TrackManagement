import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Track, TrackStatus } from '../../core/track.models';
import { TracksService } from '../../core/tracks.service';

@Component({
  imports: [DatePipe, RouterLink],
  selector: 'app-track-list',
  templateUrl: './track-list.html',
  styleUrl: './track-list.css',
})
export class TrackList implements OnInit {
  private readonly tracksService = inject(TracksService);
  protected readonly tracks = signal<Track[]>([]);
  protected readonly status = signal<TrackStatus | ''>('');
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal('');

  ngOnInit(): void {
    this.loadTracks();
  }

  protected setStatus(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    switch (value) {
      case '':
      case 'Draft':
      case 'Submitted':
      case 'Distributed':
        this.status.set(value);
        this.loadTracks();
        break;
    }
  }

  private loadTracks(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.tracksService.list(this.status()).subscribe({
      next: (tracks) => {
        this.tracks.set(tracks);
        this.isLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.status === 401
            ? 'Your session has expired. Sign in again to continue.'
            : 'We could not load your tracks. Check your connection and try again.',
        );
        this.isLoading.set(false);
      },
    });
  }

  protected retry(): void {
    this.loadTracks();
  }
}
