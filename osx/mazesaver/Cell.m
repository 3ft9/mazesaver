//
//  Cell.m
//  mazesaver
//
//  Created by Stuart Dallas on 04/12/2016.
//  Copyright © 2016 3ft9 Ltd. All rights reserved.
//

#import "Cell.h"

@implementation Cell

-(id)initWithLocation:(int)x y:(int)y scale:(int)scale xoff:(int)xoff yoff:(int)yoff {
    if (self = [super init]) {
        self.scale = scale;
        self.x = x;
        self.y = y;
        self.x_offset = xoff;
        self.y_offset = yoff;
        [self reset];
    }
    
    return self;
}

-(void)reset {
    self.wall_top = true;
    self.wall_right = true;
    self.wall_bottom = true;
    self.wall_left = true;
    self.current = false;
    self.visited = false;
}

-(void)draw {
    if (self.current) {
        [[NSColor cyanColor] set];
    } else if (self.visited) {
        [[NSColor darkGrayColor] set];
    } else {
        [[NSColor blackColor] set];
    }
    [[NSBezierPath bezierPathWithRect:NSMakeRect(self.x_offset + (self.x * self.scale) - 1, self.y_offset + (self.y * self.scale) - 1, self.scale + 2, self.scale + 2)] fill];

    [self drawLine:self.x_offset + (self.x * self.scale)
                y1:self.y_offset + (self.y * self.scale)
                x2:self.x_offset + (self.x * self.scale) + self.scale
                y2:self.y_offset + (self.y * self.scale)
           visible:self.wall_top
     ];
    [self drawLine:self.x_offset + (self.x * self.scale) + self.scale
                y1:self.y_offset + (self.y * self.scale)
                x2:self.x_offset + (self.x * self.scale) + self.scale
                y2:self.y_offset + (self.y * self.scale) + self.scale
           visible:self.wall_right
     ];
    [self drawLine:self.x_offset + (self.x * self.scale)
                y1:self.y_offset + (self.y * self.scale) + self.scale
                x2:self.x_offset + (self.x * self.scale) + self.scale
                y2:self.y_offset + (self.y * self.scale) + self.scale
           visible:self.wall_bottom
     ];
    [self drawLine:self.x_offset + (self.x * self.scale)
                y1:self.y_offset + (self.y * self.scale)
                x2:self.x_offset + (self.x * self.scale)
                y2:self.y_offset + (self.y * self.scale) + self.scale
           visible:self.wall_left
     ];
}

-(void)drawLine:(int)x1 y1:(int)y1 x2:(int)x2 y2:(int)y2 visible:(bool)visible {
    NSBezierPath *path = [NSBezierPath bezierPath];
    [path moveToPoint: NSMakePoint(x1, y1)];
    [path lineToPoint: NSMakePoint(x2, y2)];
    if (self.visited) {
        if (visible) {
            [[NSColor whiteColor] setStroke];
        } else {
            [[NSColor darkGrayColor] setStroke];
        }
    } else {
        [[NSColor blackColor] setStroke];
    }
    [path setLineWidth: 1];
    [path stroke];
}

@end
