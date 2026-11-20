import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TrackDetails } from '../../core/track.models';
import { TracksService } from '../../core/tracks.service';

@Component({
  imports: [DatePipe, RouterLink],
  selector: 'app-track-detail',
  templateUrl: './track-detail.html',
  styleUrl: './track-detail.css',
})
export class TrackDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  private readonly tracksService = inject(TracksService);
  protected readonly track = signal<TrackDetails | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal('');

  ngOnInit(): void {
    this.route.paramMap.pipe(takeUntilDestroyed(this.destroyRef)).subscribe((params) => {
      const id = Number(params.get('id'));
      if (!Number.isInteger(id) || id < 1) {
        this.errorMessage.set('This track could not be found.');
        this.isLoading.set(false);
        return;
      }
      this.loadTrack(id);
    });
  }

  protected retry(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (Number.isInteger(id) && id > 0) {
      this.loadTrack(id);
    }
  }

  private loadTrack(id: number): void {
    this.track.set(null);
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.tracksService.get(id).subscribe({
      next: (track) => {
        this.track.set(track);
        this.isLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.status === 404
            ? 'This track could not be found.'
            : error.status === 401
              ? 'Your session has expired. Sign in again to continue.'
              : 'We could not load this track. Check your connection and try again.',
        );
        this.isLoading.set(false);
      },
    });
  }
}
