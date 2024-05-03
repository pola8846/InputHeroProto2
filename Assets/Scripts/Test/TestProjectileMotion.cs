using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectileMotion : MonoBehaviour
{
    public Vector2 targetPosition; // 최종 위치
    public float gravity = 9.81f;   // 중력 가속도
    public float time = 1f;

    private Vector2 initialPosition; // 초기 위치
    private Vector2 initialVelocity; // 초기 속도 벡터

    void Start()
    {
        // 초기 위치를 설정합니다.
        initialPosition = transform.position;

        // 초기 속도 벡터를 계산합니다.
        CalculateInitialVelocity();
    }

    void CalculateInitialVelocity()
    {
        // 초기 위치와 최종 위치 사이의 거리를 계산합니다.
        Vector2 displacement = targetPosition - initialPosition;

        // 포물선 운동에서 수평 방향 속도는 변하지 않으므로 초기 속도 벡터의 x 성분은 displacement의 x 성분입니다.
        initialVelocity.x = displacement.x / time;

        // 포물선 운동에서 수직 방향 속도는 중력의 영향을 받으므로 초기 속도 벡터의 y 성분은 수직 방향으로 이동하기 위한 초기 속도입니다.
        // 초기 속도를 계산하는 수식을 사용하여 y 성분을 계산합니다.
        initialVelocity.y = (displacement.y - 0.5f * gravity * Mathf.Pow(displacement.x / initialVelocity.x, 2)) / (displacement.x / initialVelocity.x) / time;
    }

    void Update()
    {
        // 현재 시간을 가져옵니다.
        float t = Time.time;

        // 포물선 운동의 위치를 계산합니다.
        Vector2 position = initialVelocity * t + 0.5f * new Vector2(0, -gravity) * t * t + initialPosition;

        // 오브젝트를 계산된 위치로 이동시킵니다.
        transform.position = position;
    }
}
