import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-image-upload',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  template: `
    <div class="image-upload-container">
      <div class="preview-area" *ngIf="previewUrl; else uploadPlaceholder">
        <img [src]="previewUrl" alt="Receipt Preview">
        <button mat-mini-fab color="warn" class="remove-btn" (click)="removeImage($event)">
          <mat-icon>close</mat-icon>
        </button>
      </div>
      <ng-template #uploadPlaceholder>
        <div class="upload-placeholder" (click)="fileInput.click()">
          <mat-icon>add_a_photo</mat-icon>
          <span>レシート・領収書の画像を追加</span>
        </div>
      </ng-template>
      <input
        #fileInput
        type="file"
        accept="image/*"
        style="display: none"
        (change)="onFileSelected($event)"
      >
    </div>
  `,
  styles: [`
    .image-upload-container {
      width: 100%;
      height: 200px;
      border: 2px dashed rgba(255, 255, 255, 0.2);
      border-radius: 12px;
      display: flex;
      justify-content: center;
      align-items: center;
      overflow: hidden;
      background: rgba(255, 255, 255, 0.05);
      cursor: pointer;
      transition: all 0.3s ease;
    }
    .image-upload-container:hover {
      background: rgba(255, 255, 255, 0.08);
      border-color: rgba(255, 255, 255, 0.4);
    }
    .preview-area {
      position: relative;
      width: 100%;
      height: 100%;
    }
    .preview-area img {
      width: 100%;
      height: 100%;
      object-fit: contain;
    }
    .remove-btn {
      position: absolute;
      top: 8px;
      right: 8px;
    }
    .upload-placeholder {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 12px;
      color: rgba(255, 255, 255, 0.6);
    }
    .upload-placeholder mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
    }
  `]
})
export class ImageUploadComponent {
  @Input() previewUrl: string | null = null;
  @Output() imageSelected = new EventEmitter<string>();

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        const result = e.target?.result as string;
        this.previewUrl = result;
        this.imageSelected.emit(result);
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage(event: Event): void {
    event.stopPropagation();
    this.previewUrl = null;
    this.imageSelected.emit('');
  }
}
