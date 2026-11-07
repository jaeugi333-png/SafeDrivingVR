
using UnityEngine;
using TMPro;

public class TwoHandDrivingDebug : MonoBehaviour
{
    [Header("연결할 스크립트")]
    public VRSteeringWheel steering_wheel;
    public TwoHandDrivingChecker two_hand_checker;
    public CarController car_controller;

    [Header("디버그 UI")]
    public TMP_Text debug_text;

    private void LateUpdate()
    {
        if (debug_text == null)
        {
            return;
        }

        if (steering_wheel == null ||
            two_hand_checker == null ||
            car_controller == null)
        {
            debug_text.text =
                "양손 운전 디버그\n" +
                "스크립트 연결 오류";

            return;
        }

        int violationCount = 0;

        if (TwoHandDrivingRecordManager.Instance != null)
        {
            violationCount =
                TwoHandDrivingRecordManager.Instance.violation_count;
        }

        debug_text.text =
            "=== TWO HAND DEBUG ===\n" +
            "Left Grip: " + steering_wheel.LeftGrip + "\n" +
            "Right Grip: " + steering_wheel.RightGrip + "\n" +
            "Both Hands: " +
                steering_wheel.IsBothHandsGripping + "\n" +
            "Speed: " +
                car_controller.current_speed.ToString("F1") +
                " km/h\n" +
            "Measuring: " +
                two_hand_checker.is_measuring + "\n" +
            "Violation Timer: " +
                two_hand_checker.non_two_hand_duration.ToString("F1") +
                " sec\n" +
            "Violation Count: " + violationCount;
    }
}
