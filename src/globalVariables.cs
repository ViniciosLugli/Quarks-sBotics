//Data ---------------------------------------------
byte CurrentState = 0b_0000_0001;//Init in followline

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

static Sound sTurnNotGreen = new Sound("F2", 80);
static Sound sTurnGreen = new Sound("C3", 60);

static Color cFollowLine = new Color(255, 255, 255);
static Color cTurnNotGreen = new Color(0, 0, 0);
static Color cTurnGreen = new Color(0, 255, 0);
