//Data ---------------------------------------------
static Sound sTurnNotGreen = new Sound("F2", 80);
static Sound sTurnGreen = new Sound("C3", 60);
static Sound sFakeGreen = new Sound("G", 100);
static Sound sAlertOffline = new Sound("D#", 100);
static Sound sFindLine = new Sound("C1", 50);
static Sound sMultiplesCross = new Sound("E", 80);
static Sound sRescueFindArea = new Sound("C2", 120);
static Sound sLifting = new Sound("G", 120);

static Color cFollowLine = new Color(255, 255, 255);
static Color cTurnNotGreen = new Color(0, 0, 0);
static Color cTurnGreen = new Color(0, 255, 0);
static Color cFakeGreen = new Color(255, 255, 0);
static Color cAlertOffline = new Color(255, 0, 0);
static Color cRampFollowLine = new Color(255, 0, 255);

static int SETUPTIME = Time.current.millis;

static byte UNIQUEID = 0;
