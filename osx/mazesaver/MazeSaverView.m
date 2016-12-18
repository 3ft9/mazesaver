//
//  mazesaverView.m
//  mazesaver
//
//  Created by Stuart Dallas on 04/12/2016.
//  Copyright © 2016 3ft9 Ltd. All rights reserved.
//

#import "MazeSaverView.h"

@implementation MazeSaverView

- (int)getIndex:(int)x y:(int)y {
    if (x < 0 || x > self.width-1 || y < 0 || y > self.height-1 ) {
        return -1;
    }
    return (y * self.width) + x;
}

- (Cell *)getCellAt:(int)x y:(int)y {
    int index = [self getIndex:x y:y];
    if (index < 0 || index >= self.cells.count) {
        return NULL;
    }
    return (Cell *)self.cells[index];
}

- (instancetype)initWithFrame:(NSRect)frame isPreview:(BOOL)isPreview
{
    self = [super initWithFrame:frame isPreview:isPreview];
    if (self) {
        [self setAnimationTimeInterval:1/120.0];
        
        self.initialised = false;
        self.finished = 0;
        self.scale = 40;
        self.width = floor((self.bounds.size.width - 20) / self.scale);
        self.height = floor((self.bounds.size.height - 20) / self.scale);
        
        self.path = [[NSMutableArray alloc] init];
        
        int xoff = floor((self.bounds.size.width - (self.width * self.scale)) / 2);
        int yoff = floor((self.bounds.size.height - (self.height * self.scale)) / 2);
        
        NSMutableArray *cells = [NSMutableArray array];
        for (int y = 0; y < self.height; y++) {
            for (int x = 0; x < self.width; x++) {
                [cells addObject:[[Cell alloc] initWithLocation:x y:y scale:self.scale xoff:xoff yoff:yoff]];
            }
        }
        
        self.cells = cells;
        
        self.indexes = [NSMutableArray arrayWithCapacity:self.cells.count];

        self.running = false;
    }
    return self;
}

- (void)startAnimation
{
    [super startAnimation];
}

- (void)stopAnimation
{
    [super stopAnimation];
}

- (void)animateOneFrame
{
    if (!self.running) {
        NSTimeInterval now = [[NSDate date] timeIntervalSince1970];
        if (now - self.finished > 2) {
            if (self.initialised) {
                if (self.indexes.count == 0) {
                    for (long i = 0; i < self.cells.count; i++) {
                        [self.indexes addObject:[NSNumber numberWithInteger:i]];
                    }
                }
                
                int num = floor(self.cells.count / 60);
                while (num > 0 && self.indexes.count > 0) {
                    int idx = SSRandomIntBetween(0, (int)self.indexes.count - 1);
                    Cell *c = [self.cells objectAtIndex:[(NSNumber *)[self.indexes objectAtIndex:idx] integerValue]];
                    [c reset];
                    [c draw];
                    [self.indexes removeObjectAtIndex:idx];
                    num--;
                }
            }
            
            if (self.indexes.count == 0) {
                // Pick a random starting cell.
                self.current = (Cell *)self.cells[SSRandomIntBetween(0, (int)self.cells.count-1)];
                self.current.current = true;
                self.current.visited = true;
                self.running = true;
                self.initialised = true;
            }
        }
    } else {
        NSMutableArray* neighbours = [NSMutableArray arrayWithCapacity:4];

        Cell *top = [self getCellAt:self.current.x y:self.current.y-1];
        Cell *right = [self getCellAt:self.current.x+1 y:self.current.y];
        Cell *bottom = [self getCellAt:self.current.x y:self.current.y + 1];
        Cell *left = [self getCellAt:self.current.x-1 y:self.current.y];
        
        if (top && !top.visited) {
            [neighbours addObject:top];
        }
        if (right && !right.visited) {
            [neighbours addObject:right];
        }
        if (bottom && !bottom.visited) {
            [neighbours addObject:bottom];
        }
        if (left && !left.visited) {
            [neighbours addObject:left];
        }
        
        if (neighbours.count == 0) {
            // Backtrack.
            if (self.path.count > 0) {
                self.current.current = false;
                [self.current draw];
                self.current = (Cell *)[self.path objectAtIndex:self.path.count-1];
                [self.path removeLastObject];
                self.current.current = true;
                [self.current draw];
            } else {
                self.running = false;
                self.finished = [[NSDate date] timeIntervalSince1970];
            }
        } else {
            // Pick one at random.
            Cell *chosen = (Cell *)neighbours[SSRandomIntBetween(0, (int)neighbours.count)];
            // Remove walls.
            if (self.current.x - chosen.x == -1) {
                self.current.wall_right = false;
                chosen.wall_left = false;
            } else if (self.current.x - chosen.x == 1) {
                self.current.wall_left = false;
                chosen.wall_right = false;
            } else if (self.current.y - chosen.y == -1) {
                self.current.wall_bottom = false;
                chosen.wall_top = false;
            } else if (self.current.y - chosen.y == 1) {
                self.current.wall_top = false;
                chosen.wall_bottom = false;
            }
            // Set the flags.
            self.current.current = false;
            // Draw it.
            [self.current draw];
            // Add it to the path.
            [self.path addObject:self.current];
            // Switch the current.
            self.current = chosen;
            self.current.current = true;
            self.current.visited = true;
            // Draw it.
            [self.current draw];
        }
    }
}

- (BOOL)hasConfigureSheet
{
    return NO;
}

- (NSWindow*)configureSheet
{
    return nil;
}

@end
