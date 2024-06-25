using UnityEngine;

public class CameraBoundsExample : MonoBehaviour
{
    public Camera mainCamera;
    public Transform character;
    public float zDistance
    {
        get
        {
            return -mainCamera.transform.position.z;
        }
    }

    void Update()
    {
        // 카메라의 뷰포트 경계 계산
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        // X, Y 경계 설정
        float minX = bottomLeft.x;
        float maxX = topRight.x;
        float minY = bottomLeft.y;
        float maxY = topRight.y;

        // 캐릭터의 현재 위치 가져오기
        Vector3 characterPosition = character.position;

        // 캐릭터가 카메라 뷰포트를 벗어나지 않도록 위치 제한
        characterPosition.x = Mathf.Clamp(characterPosition.x, minX, maxX);
        characterPosition.y = Mathf.Clamp(characterPosition.y, minY, maxY);

        // 캐릭터 위치 갱신
        character.position = characterPosition;
    }
}
