<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes"/>

<xsl:variable name="page_id">4387</xsl:variable>

<xsl:template match="/">
  <xsl:text disable-output-escaping="yes"><![CDATA[
<style type="text/css">
table.SeatList { margin-left: auto; margin-right: auto; }
td.SeatElement { text-align: center; }
</style>
]]></xsl:text>


  <!-- If they have not adopted any seats, display a helpful message. -->
  <xsl:if test="count(sql/rows/row) = 0">
    <div>
      <xsl:text>It seems you have not adopted any seats yet.</xsl:text>
    </div>
  </xsl:if>
  
  <!-- If they have adopted only a single seat, move along home. -->
  <xsl:if test="count(sql/rows/row) = 1">
    <xsl:text disable-output-escaping="yes"><![CDATA[
    <script type="text/javascript">
    window.location = "default.aspx?page=]]></xsl:text><xsl:value-of select="$page_id" />
    <xsl:text disable-output-escaping="yes">&amp;seat_pledge_id=</xsl:text>
    <xsl:value-of select="sql/rows/row/seat_pledge_id"></xsl:value-of><xsl:text disable-output-escaping="yes"><![CDATA[";
    </script>
    ]]></xsl:text>
  </xsl:if>

  <!-- Walk each SQL row (seat) and put it in a table so it displays nice. -->  
  <table border="0" class="SeatList">
    <tr class="SeatList">
      <xsl:for-each select="sql/rows/row">
        <td class="SeatElement">
          <a>
            <xsl:attribute name="href">
              <xsl:text>default.aspx?page=</xsl:text>
              <xsl:copy-of select="$page_id" />
              <xsl:text disable-output-escaping="yes">&amp;seat_pledge_id=</xsl:text>
              <xsl:value-of select="seat_pledge_id" />
            </xsl:attribute>
            <img src="UserControls/Custom/SOTHC/MiTM/Images/mitm_chair.jpg" width="200" />
            <div style="margin-top: 16px; font-size: 16px;">
              <xsl:text>Seat </xsl:text>
              <xsl:value-of select="seat" />
            </div>
          </a>
        </td>

        <xsl:if test="position() mod 3 = 0">
          <xsl:text disable-output-escaping="yes">&lt;/tr&gt;&lt;tr class="SeatList"&gt;</xsl:text>
        </xsl:if>
      </xsl:for-each>
    </tr>
  </table>
</xsl:template>
</xsl:stylesheet>
