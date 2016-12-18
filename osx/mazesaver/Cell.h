//
//  Cell.h
//  mazesaver
//
//  Created by Stuart Dallas on 04/12/2016.
//  Copyright © 2016 3ft9 Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <ScreenSaver/ScreenSaver.h>

@interface Cell : NSObject

@property int scale;
@property int x;
@property int y;
@property int x_offset;
@property int y_offset;

@property bool wall_top;
@property bool wall_right;
@property bool wall_bottom;
@property bool wall_left;

@property bool current;
@property bool visited;

-(id)initWithLocation:(int)x y:(int)y scale:(int)scale xoff:(int)xoff yoff:(int)yoff;
-(void)reset;
-(void)draw;
-(void)drawLine:(int)x1 y1:(int)y1 x2:(int)x2 y2:(int)y2 visible:(bool)visible;

@end
