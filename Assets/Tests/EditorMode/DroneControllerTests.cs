using Drones;
using NUnit.Framework;
using UnityEngine;

public class DroneControllerTests
{
    private readonly GameObject _dronePrefab =
        UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

    private GameObject _drone;
    private static DroneController _droneController;
    private static DroneInputs _inputs;

    [SetUp]
    public void SetUp()
    {
        _drone = Object.Instantiate(_dronePrefab);
        _droneController = _drone.GetComponent<DroneController>();
        _droneController.Awake();
        _inputs = _drone.GetComponent<DroneInputs>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_drone);
    }

    // [Test]
    // public void TestConstruct()
    // {
    //     _droneController.Construct();
    //     Assert.NotNull(_droneController.Rb);
    //     Assert.NotNull(_droneController.Input);
    // }

    #region Move To Tests

    [Test]
    public void _0_Move_To_Set_The_Wanted_Position()
    {
        _droneController.MoveTo(Vector3.zero);
        Assert.AreEqual(Vector3.zero, _droneController.wantedPosition);
    }

    [Test]
    public void Move_To_A_Position_Sets_The_Wanted_Position()
    {
        var p = new Vector3(1f, 2f, 3f);
        _droneController.MoveTo(p);
        Assert.AreEqual(p, _droneController.wantedPosition);
    }

    [Test]
    public void Move_To_A_Negative_Position_Sets_The_Wanted_Position()
    {
        var p = new Vector3(-100f, -245f, -803f);
        _droneController.MoveTo(p);
        Assert.AreEqual(p, _droneController.wantedPosition);
    }

    [Test]
    public void Stop_Move_Sets_All_Inputs_To_Zeros()
    {
        _droneController.StopMove();
        Assert.AreEqual(Vector2.zero, _inputs.Cyclic);
        Assert.AreEqual(0f, _inputs.Pedals);
        Assert.AreEqual(0f, _inputs.Throttle);
    }

    #endregion


    [Test]
    public void Test_At_The_Wanted_Position_Without_Any_Displacement()
    {
        _droneController.MoveTo(Vector3.zero);
        Assert.IsTrue(_droneController.IsAtWantedPosition());
    }

    #region GetDirection Method

    [Test]
    public void Get_Direction_From_A_Position_In_Vector3()
    {
        var pos = new Vector3(-6, 3, 0);
        _droneController.MoveTo(pos);
        var direction = _droneController.GetDirection();
        Assert.AreEqual(new Vector3(-1, 1, 0), direction);
    }

    [Test]
    public void Get_Direction_From_Position_0()
    {
        _droneController.MoveTo(Vector3.zero);
        Assert.AreEqual(Vector3.zero, _droneController.GetDirection());
    }

    [Test]
    public void Get_Direction_From_Neg_Pos()
    {
        _droneController.MoveTo(new Vector3(-6, -9, -25978));
        Assert.AreEqual(new Vector3(-1, -1, -1), _droneController.GetDirection());
    }

    [Test]
    public void Get_Direction_From_Positive_Pos()
    {
        _droneController.MoveTo(new Vector3(85, 9, 27));
        Assert.AreEqual(Vector3.one, _droneController.GetDirection());
    }

    #endregion

    public class GoDirectionMethods //IMPROVE:To refactor if these methods move to a utility class
    {
        [Test]
        public void Go_Up_Set_Throttle_1()
        {
            _droneController.GoUp();
            Assert.AreEqual(1, _inputs.Throttle);
        }

        [Test]
        public void Go_Down_Set_Throttle_Neg_1()
        {
            _droneController.GoDown();
            Assert.AreEqual(-1, _inputs.Throttle);
        }

        [Test]
        public void Go_Right_Set_Cyclic_X_To_1()
        {
            _droneController.GoRight();
            Assert.AreEqual(1, _inputs.Cyclic.x);
        }

        [Test]
        public void Go_Left_Set_Cyclic_X_To_Neg_1()
        {
            _droneController.GoLeft();
            Assert.AreEqual(-1, _inputs.Cyclic.x);
        }

        [Test]
        public void Go_Forward_Set_Cyclic_Y_To_1()
        {
            _droneController.GoForward();
            Assert.AreEqual(1, _inputs.Cyclic.y);
        }

        [Test]
        public void Go_Backward_Set_Cyclic_Y_To_Neg_1()
        {
            _droneController.GoBackward();
            Assert.AreEqual(-1, _inputs.Cyclic.y);
        }

        [Test]
        public void Turn_Right_Set_Pedals_To_1()
        {
            _droneController.TurnRight();
            Assert.AreEqual(1, _inputs.Pedals);
        }

        [Test]
        public void Turn_Left_Set_Pedals_To_Neg_1()
        {
            _droneController.TurnLeft();
            Assert.AreEqual(-1, _inputs.Pedals);
        }
    }
}