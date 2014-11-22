using System;
using System.IO;
using System.Collections.Generic;

namespace TDGame.Kommunikation
{
	// (C) Sebastian Neubauer
	/// <summary>Ermöglicht das Schreiben in mehrere BinaryWriter gleichzeitig</summary>
	public class BinaryWriterCollection : LockedBinaryWriter
	{
		/// <summary>Die Liste von BinaryWritern, in die gleichzeitig geschrieben werden soll</summary>
		public List<Verwaltung> con_list;

		/// <summary>Erstellt eine neue BWC mit der Spielerliste an die gesendet werden soll</summary>
		public BinaryWriterCollection(List<Verwaltung> _con_list)
		{
			con_list = _con_list;
		}

		/// <summary>Schreibt einen 1-Byte-Boolean-Wert in den aktuellen Stream, wobei 0 (null) false und 1 true darstellt.</summary>
		/// <param name="value">Der zu schreibende Boolean-Wert (0 (null) oder 1).</param>
		public override void Write(bool value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt ein Byte ohne Vorzeichen in den aktuellen Stream und erhöht die aktuelle Position im Stream um ein Byte.</summary>
		/// <param name="value">Das zu schreibende Byte ohne Vorzeichen.</param>
		public override void Write(byte value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt ein Bytearray in den zugrunde liegenden Stream.</summary>
		/// <param name="buffer">Ein Bytearray, das die zu schreibenden Daten enthält.</param>
		public override void Write(byte[] buffer)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(buffer);
		}

		/// <summary>Schreibt einen Bereich eines Bytearrays in den aktuellen Stream.</summary>
		/// <param name="buffer">Ein Bytearray, das die zu schreibenden Daten enthält.</param>
		/// <param name="index">Der Anfangspunkt im buffer, an dem mit dem Schreiben begonnen wird.</param>
		/// <param name="count">Die Anzahl der zu schreibenden Bytes.</param>
		public override void Write(byte[] buffer, int index, int count)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(buffer, index, count);
		}

		/// <summary><para>Schreibt ein Unicode-Zeichen in den aktuellen Stream und erhöht die aktuelle Position im Stream</para>
		/// <para>in Abhängigkeit von der verwendeten Encoding und der in den Stream geschriebenen Zeichen.</para></summary>
		/// <param name="ch">Das zu schreibende Unicode-Zeichen (nicht-Ersatzzeichen).</param>
		public override void Write(char ch)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(ch);
		}

		/// <summary><para>Schreibt ein Zeichenarray in den aktuellen Stream und erhöht die aktuelle Position im Stream</para>
		/// <para>in Abhängigkeit von der verwendeten Encoding und der in den Stream geschriebenen Zeichen.</para></summary>
		/// <param name="chars">Ein Zeichenarray mit den zu schreibenden Daten.</param>
		public override void Write(char[] chars)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(chars);
		}

		/// <summary><para>Schreibt einen Bereich eines Zeichenarrays in den aktuellen Stream und erhöht die aktuelle Position im Stream</para>
		/// <para>in Abhängigkeit von der verwendeten Encoding und ggf. der in den Stream geschriebenen Zeichen.</para></summary>
		/// <param name="chars">Ein Zeichenarray mit den zu schreibenden Daten.</param>
		/// <param name="index">Der Anfangspunkt im buffer, an dem mit dem Schreiben begonnen wird.</param>
		/// <param name="count">Die Anzahl der zu schreibenden Zeichen.</param>
		public override void Write(char[] chars, int index, int count)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(chars, index, count);
		}

		/// <summary>Schreibt einen Dezimalwert in den aktuellen Stream und erhöht die Position im Stream um 16 Bytes.</summary>
		/// <param name="value">Der zu schreibende Dezimalwert.</param>
		public override void Write(decimal value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt einen 8-Byte-Gleitkommawert in den aktuellen Stream und erhöht die Position im Stream um 8 Bytes.</summary>
		/// <param name="value">Der zu schreibende 8-Byte-Gleitkommawert.</param>
		public override void Write(double value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt einen 4-Byte-Gleitkommawert in den aktuellen Stream und erhöht die Position im Stream um 4 Bytes.</summary>
		/// <param name="value">Der zu schreibende 4-Byte-Gleitkommawert.</param>
		public override void Write(float value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt eine 4-Byte-Ganzzahl mit Vorzeichen in den aktuellen Stream und erhöht die Position im Stream um 4 Bytes.</summary>
		/// <param name="value">Die zu schreibende 4-Byte-Ganzzahl mit Vorzeichen.</param>
		public override void Write(int value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt eine 8-Byte-Ganzzahl mit Vorzeichen in den aktuellen Stream und erhöht die Position im Stream um 8 Bytes.</summary>
		/// <param name="value">Die zu schreibende 8-Byte-Ganzzahl mit Vorzeichen.</param>
		public override void Write(long value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt ein Byte mit Vorzeichen in den aktuellen Stream und erhöht die aktuelle Position im Stream um ein Byte.</summary>
		/// <param name="value">Das zu schreibende Byte mit Vorzeichen.</param>
		public override void Write(sbyte value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt eine 2-Byte-Ganzzahl mit Vorzeichen in den aktuellen Stream und erhöht die Position im Stream um 2 Bytes.</summary>
		/// <param name="value">Die zu schreibende 2-Byte-Ganzzahl mit Vorzeichen.</param>
		public override void Write(short value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary><para>Schreibt eine Zeichenfolge mit Längenpräfix in der aktuellen Codierung von System.IO.BinaryWriter in diesen Stream</para>
		/// <para>und erhöht die aktuelle Position im Stream in Abhängigkeit von der verwendeten Codierung</para>
		/// <para>und der in den Stream geschriebenen Zeichen.</para></summary>
		/// <param name="value">Der zu schreibende Wert.</param>
		public override void Write(string value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt eine 4-Byte-Ganzzahl ohne Vorzeichen in den aktuellen Stream und erhöht die Position im Stream um 4 Bytes.</summary>
		/// <param name="value">Die zu schreibende 4-Byte-Ganzzahl ohne Vorzeichen.</param>
		public override void Write(uint value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt eine 8-Byte-Ganzzahl ohne Vorzeichen in den aktuellen Stream und erhöht die Position im Stream um 8 Bytes.</summary>
		/// <param name="value">Die zu schreibende 8-Byte-Ganzzahl ohne Vorzeichen.</param>
		public override void Write(ulong value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Schreibt eine 2-Byte-Ganzzahl ohne Vorzeichen in den aktuellen Stream und erhöht die Position im Stream um 2 Bytes.</summary>
		/// <param name="value">Die zu schreibende 2-Byte-Ganzzahl ohne Vorzeichen.</param>
		public override void Write(ushort value)
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Write(value);
		}

		/// <summary>Sperrt die BWC und sichert somit somit den Zugriff</summary>
		public override void Lock()
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Lock();
		}

		/// <summary>Gibt die BWC wieder frei</summary>
		public override void Unlock()
		{
			foreach (Verwaltung p in con_list)
				p.streamw.Unlock();
		}
	}
}