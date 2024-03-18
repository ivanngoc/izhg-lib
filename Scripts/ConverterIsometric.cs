using System;


namespace IziHardGames.Libs.NonEngine.ProjectionTools
{
	public class ConverterProjectionOrtho
	{


		/// <summary>
		/// align object to given axis
		/// </summary>
		/// <param name="xAxis"></param>
		/// <param name="yAxis"></param>
		/// <param name="zAxis"></param>
		/// <returns>
		/// Rotation For X, Y, Z To set. (Not add or multiply)
		/// </returns>
		public (float, float, float) GetRotationToProjectionAxis(float xAxis, float yAxis, float zAxis)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc cref="GetRotationToProjectionAxis(float, float, float)"/>
		/// <returns>
		/// Quaternion to Set. (Not add or multiply)
		/// </returns>
		public (float, float, float, float) RotateobjectByQuaternion(float xAxis, float yAxis, float zAxis) => throw new NotImplementedException();

	}
	public class ConverterProjectionPerspective
	{ }

	public class ConverterIsometric
	{
		/// <summary>
		/// длина проекции единичного отрезка на оси X или Z в изометрии.  ≈сли смотреть на ровносто¤щий куб то это верхн¤¤ и нижн¤¤ стороны после поворота в изомметрию
		/// </summary>
		public const float HORIZONTAL_PORTITION_DISTORTION = 0.8660254037844386f;
		public const float HORIZONTAL_PORTITION_DISTORTION_REVERSE = 0.1339745962155614f;
		/// <summary>
		/// полн¤ ширина куба после вращени¤ в изометрию. –ассто¤ни¤ от крайней прайвой до крайней левой стороны
		/// </summary>
		public const float WIDTH_ISONETRIC = HORIZONTAL_PORTITION_DISTORTION * 2;
		public const float VERTICAL_PORTITION_DISTORTION = default;
		/// <summary>
		/// ¬ысота от крайней нижней точки куба в изометрии линии до 
		/// </summary>
		public const float HEIGHT_FROM_BOT_TO_DEGREE_30_BOT = 1 / 2;

		/// <summary>
		/// ѕолучить искажени¤ длин единичных осей относительно начальных положений (x-право, y - вверх, z - вперед).  положительный - значит удлинилс¤, отрицательный - сжалс¤/укоротилс¤
		///
		/// </summary>
		/// <returns></returns>
		public (float, float, float) GetRelativeDistortionsPerAxis()
		{
			throw new NotImplementedException();
			//return (HORIZONTAL_PORTITION_DISTORTION, -, -HORIZONTAL_PORTITION_DISTORTION_REVERSE);
		}

		/// <summary>
		/// Origins for AXIS: X to RIGHT, Y to UP, Z to Forward <br/>
		/// Roation Order For XYZ. Isometric is (30,90,30). <br/>
		/// yAxis look up for viewer, but for <br/>
		/// xAxis look to right with 30 degree up from horizontal line <br/>
		/// zAxis look to left with 30 degree up from horizontal line<br/>
		/// From viewer look at screen
		/// </summary>
		/// <see cref="https://en.wikipedia.org/wiki/Isometric_projection#overview"/>
		public (float, float, float) GetIsometricRotationXYZ()
		{
			return (335.821014f, 309.414673f, 26.6769924f);
		}
		public (float, float, float) GetIsometricRotationXYZ_Signed()
		{
			return (-24.1789856f, -50.5853271f, 26.6769924f);
		}
	}
}