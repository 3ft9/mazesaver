//
//  mazesaverView.h
//  mazesaver
//
//  Created by Stuart Dallas on 04/12/2016.
//  Copyright © 2016 3ft9 Ltd. All rights reserved.
//

#import <ScreenSaver/ScreenSaver.h>
#import "Cell.h"

@interface MazeSaverView : ScreenSaverView

{
    IBOutlet id configSheet;
}

@property bool running;
@property bool initialised;
@property int scale;
@property int width;
@property int height;
@property NSArray* cells;
@property Cell* current;
@property NSMutableArray* path;
@property NSMutableArray* indexes;

@property NSTimeInterval finished;

- (int)getIndex:(int)x y:(int)y;
- (Cell *)getCellAt:(int)x y:(int)y;

@end
