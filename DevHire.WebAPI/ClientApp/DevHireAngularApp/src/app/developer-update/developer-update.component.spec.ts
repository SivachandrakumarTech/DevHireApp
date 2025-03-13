import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeveloperUpdateComponent } from './developer-update.component';

describe('DeveloperUpdateComponent', () => {
  let component: DeveloperUpdateComponent;
  let fixture: ComponentFixture<DeveloperUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeveloperUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeveloperUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
