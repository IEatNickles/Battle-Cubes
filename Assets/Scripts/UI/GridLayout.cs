using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.UI;
using UnityEngine;

public class GridLayout : LayoutGroup
{
	public enum FitType
	{
		Uniform,
		Width,
		Height,
		FixedRows,
		FixedColmns
	}

	public FitType fitType;

	[Min(1)]public int rows;
	[Min(1)]public int columns;
	public float2 cellSize;
	public float2 spacing;

	public bool fitX;
	public bool fitY;

	[Space]

	public bool constrainX;
	public bool constrainY;

	[Space]

	[Min(0)] public float minWidthConstraint;
	[Min(0)] public float minHeightConstraint;

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();

		if (fitType == FitType.Height || fitType == FitType.Width || fitType == FitType.Uniform)
		{
			fitX = true;
			fitY = true;

			float sqrRt = math.sqrt(transform.childCount);
			rows = (int)math.ceil(sqrRt);
			columns = (int)math.ceil(sqrRt);
		}

		float parentWidth = rectTransform.rect.width;
		float parentHeight = rectTransform.rect.height;

		if (fitType == FitType.Width || fitType == FitType.FixedColmns)
		{
			rows = (int)math.ceil(transform.childCount / (float)columns);
		}
		else if (fitType == FitType.Height || fitType == FitType.FixedRows)
		{
			columns = (int)math.ceil(transform.childCount / (float)rows);
		}
			
		float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
		float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

		cellSize.x = fitX ? cellWidth : cellSize.x;
		cellSize.y = fitY ? cellHeight : cellSize.y;

		if (constrainX || constrainY)
		{
			Vector2 size = rectTransform.rect.size;
			Vector2 min = new Vector2(((cellSize.x + spacing.x) * columns) - spacing.x + padding.left + padding.right,
										((cellSize.y + spacing.y) * rows) - spacing.y + padding.top + padding.bottom);

			if (constrainX) size.x = math.max(min.x, minWidthConstraint);
			if (constrainY) size.y = math.max(min.y, minHeightConstraint);

			rectTransform.sizeDelta = size;
		}

		int rowCount = 0;
		int columnCount = 0;

		for (int i = 0; i < rectChildren.Count; i++)
		{
			rowCount = i / columns;
			columnCount = i % columns;

			var item = rectChildren[i];

			var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
			var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

			SetChildAlongAxis(item, 0, xPos, cellSize.x);
			SetChildAlongAxis(item, 1, yPos, cellSize.y);
		}
	}

	public override void CalculateLayoutInputVertical()
	{
	}

	public override void SetLayoutHorizontal()
	{
	}

	public override void SetLayoutVertical()
	{
	}
}
