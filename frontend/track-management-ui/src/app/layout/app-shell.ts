import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../core/auth.service';

@Component({
  imports: [RouterLink, RouterOutlet],
  selector: 'app-shell',
  templateUrl: './app-shell.html',
  styleUrl: './app-shell.css',
})
export class AppShell {
  protected readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  signOut(): void {
    this.auth.logout();
    void this.router.navigate(['/login']);
  }
}
