﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.IDAL;
using Apps.Models;
using System.Data.Entity;

namespace Apps.DAL
{
    public class SysModuleRepository : ISysModuleRepository,IDisposable
    {
        public IQueryable<SysModule> GetList(DBContainer db)
        {
            return db.SysModule.AsQueryable();
        }
        public IQueryable<SysModule> GetModuleBySystem(DBContainer db, string parentId)
        {
            IQueryable<SysModule> entityList = db.SysModule.AsQueryable();
            return entityList.Where(r => r.ParentId == parentId);
        }
        public int Create(SysModule entity)
        {
            using (DBContainer db = new DBContainer())
            {
                db.Set<SysModule>().Add(entity);
                return db.SaveChanges();
            }
        }
        public void Delete(DBContainer db, string id)
        {
            SysModule entity = db.SysModule.SingleOrDefault(r => r.Id == id);
            if ( entity != null )
            {
                //删除SysRight表数据
                var rightList = db.SysRight.AsQueryable().Where(r => r.ModuleId == id);
                foreach( var right in rightList )
                {
                    //删除SysRightOperate表数据
                    var rightOperateList = db.SysRightOperate.AsQueryable().Where(r => r.RightId == right.Id);
                    foreach( var rightOperate in rightOperateList )
                    {
                        db.Set<SysRightOperate>().Remove(rightOperate);
                    }
                    db.Set<SysRight>().Remove(right);
                }
                //删除SysModuleOperate数据
                var moduleOperateList = db.SysModuleOperate.AsQueryable().Where(r => r.ModuleId == id);
                foreach( var moduleOperate in moduleOperateList )
                {
                    db.Set<SysModuleOperate>().Remove(moduleOperate);
                }
                //删除SysModule表数据
                db.Set<SysModule>().Remove(entity);
                //db.SaveChanges();
            }
        }
        public int Edit(SysModule entity)
        {
            using (DBContainer db = new DBContainer())
            {
                db.Set<SysModule>().Attach(entity);
                db.Entry<SysModule>(entity).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }
        public SysModule GetById(string id)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.SysModule.SingleOrDefault(r => r.Id == id);
            }
        }
        public bool IsExist(string id)
        {
            using (DBContainer db = new DBContainer())
            {
                SysModule entity = db.SysModule.SingleOrDefault(r => r.Id == id);
                if (entity != null)
                    return true;
                return false;
            }
        }
        public void Dispose()
        {

        }
    }
}
