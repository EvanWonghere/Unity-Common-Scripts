import pandas as pd
import json
import os

def excel_to_json():
    # 提示用户输入 Excel 文件名
    excel_filename = input("请输入 Excel 文件名（包括扩展名，如 data.xlsx）：")
    
    # 检查文件是否存在
    if not os.path.exists(excel_filename):
        print(f"文件 {excel_filename} 不存在，请检查文件路径。")
        return

    # 读取 Excel 文件
    try:
        df = pd.read_excel(excel_filename)
    except Exception as e:
        print(f"读取 Excel 文件时发生错误：{e}")
        return

    # 获取文件名（不包括扩展名）
    json_filename = os.path.splitext(excel_filename)[0] + '.json'

    # 将 DataFrame 转换为 JSON 格式
    try:
        json_data = df.to_json(orient='records', force_ascii=False)
    except Exception as e:
        print(f"转换为 JSON 时发生错误：{e}")
        return

    # 将 JSON 数据写入文件
    try:
        with open(json_filename, 'w', encoding='utf-8') as json_file:
            json_file.write(json_data)
        print(f"转换成功，JSON 文件已保存为 {json_filename}")
    except Exception as e:
        print(f"写入 JSON 文件时发生错误：{e}")

if __name__ == "__main__":
    excel_to_json()

