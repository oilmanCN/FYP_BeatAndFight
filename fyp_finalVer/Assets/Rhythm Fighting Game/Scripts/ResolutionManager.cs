using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    void Start()
    {
        // 初始化下拉菜单选项
        PopulateResolutionDropdown();

        // 添加监听器，当下拉菜单值改变时调用SetResolution方法
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void PopulateResolutionDropdown()
    {
        // 获取系统支持的分辨率
        Resolution[] resolutions = Screen.resolutions;

        // 清空下拉菜单
        resolutionDropdown.ClearOptions();

        // 添加分辨率选项
        foreach (Resolution resolution in resolutions)
        {
            // 添加分辨率和刷新率到下拉菜单
            resolutionDropdown.options.Add(new Dropdown.OptionData($"{resolution.width} x {resolution.height} ({resolution.refreshRate}Hz)"));
        }

        // 设置默认选项
        resolutionDropdown.value = FindCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
    }

    int FindCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width == currentResolution.width &&
                Screen.resolutions[i].height == currentResolution.height &&
                Screen.resolutions[i].refreshRate == currentResolution.refreshRate)
            {
                return i;
            }
        }

        return 0; // 默认返回第一个分辨率
    }

    // 在下拉菜单中选择了新的分辨率时调用
    void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }
}
