//Private robot info. current robo-4:
private const byte kLights = 3;//number of sensors
private const byte kUltrassonics = 2;//number of sensors
private const byte kRefreshRate = 31;//ms of refresh rate in color/light sensor
//

//Data ---------------------------------------------
static byte CurrentState = 0b_0000_0001;//Init in followline

enum States : byte {
    FOLLOWLINE = 1 << 0,
    OBSTACLE = 1 << 1,
    UPRAMP = 1 << 2,
    DOWNRAMP = 1 << 3,
    RESCUERAMP = 1 << 4,
    RESCUE = 1 << 5,
    RESCUEEXIT = 1 << 6,
    NOP = 1 << 7
}
