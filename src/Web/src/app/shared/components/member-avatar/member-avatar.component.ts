import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-member-avatar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="avatar" [style.background-color]="color()" [title]="name()">
      {{ initials() }}
    </div>
  `,
  styles: [`
    .avatar {
      width: 36px;
      height: 36px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      color: white;
      font-weight: bold;
      font-size: 14px;
      border: 2px solid rgba(255, 255, 255, 0.2);
    }
  `]
})
export class MemberAvatarComponent {
  name = input<string>('');
  color = input<string>('#3f51b5');

  initials = computed(() => {
    const n = this.name();
    return n ? n.charAt(0).toUpperCase() : '?';
  });
}
