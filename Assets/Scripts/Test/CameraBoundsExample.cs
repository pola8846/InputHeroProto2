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
        // ī�޶��� ����Ʈ ��� ���
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        // X, Y ��� ����
        float minX = bottomLeft.x;
        float maxX = topRight.x;
        float minY = bottomLeft.y;
        float maxY = topRight.y;

        // ĳ������ ���� ��ġ ��������
        Vector3 characterPosition = character.position;

        // ĳ���Ͱ� ī�޶� ����Ʈ�� ����� �ʵ��� ��ġ ����
        characterPosition.x = Mathf.Clamp(characterPosition.x, minX, maxX);
        characterPosition.y = Mathf.Clamp(characterPosition.y, minY, maxY);

        // ĳ���� ��ġ ����
        character.position = characterPosition;
    }
}
